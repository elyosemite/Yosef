import gleam/json
import domain/claim.{Event, ClaimCreated, ClaimUpdated, ClaimApproved, ClaimRejected, Money, ClaimDetails}
import gleam/option.{None, Some}
import gleam/result
import gleam/string

pub fn encode(event: Event) -> #(String, json.Json) {
  case event {
    ClaimCreated(policy_id, amount, details) -> {
      #("ClaimCreated", json.object([
        #("policy_id", json.string(policy_id)),
        #("amount", json.object([#("amount", json.float(amount.amount)), #("currency", json.string(amount.currency))])),
        #("details", json.object([#("description", json.string(details.description)), #("date", json.string(details.date))])),
      ]))
    }
    ClaimUpdated(details, amount_opt) -> {
      #("ClaimUpdated", json.object([
        #("details", json.object([#("description", json.string(details.description)), #("date", json.string(details.date))])),
        #("amount", case amount_opt {
          Some(a) -> json.object([#("amount", json.float(a.amount)), #("currency", json.string(a.currency))])
          None -> json.null()
        }),
      ]))
    }
    ClaimApproved -> #("ClaimApproved", json.null())
    ClaimRejected -> #("ClaimRejected", json.null())
  }
}

// Decode from event_type and json string to Event
pub fn decode(event_type: String, data: String) -> Result(Event, String) {
  use decoded <- result.try(json.decode(data) |> result.map_error(fn(_) { "Invalid JSON" }))
  case event_type {
    "ClaimCreated" -> {
      use policy_id <- result.try(json.string_at(decoded, "policy_id") |> result.map_error(fn(_) { "Missing policy_id" }))
      use amount_obj <- result.try(json.object_at(decoded, "amount") |> result.map_error(fn(_) { "Missing amount" }))
      use amount_val <- result.try(json.float_at(amount_obj, "amount") |> result.map_error(fn(_) { "Missing amount.amount" }))
      use currency <- result.try(json.string_at(amount_obj, "currency") |> result.map_error(fn(_) { "Missing amount.currency" }))
      use details_obj <- result.try(json.object_at(decoded, "details") |> result.map_error(fn(_) { "Missing details" }))
      use description <- result.try(json.string_at(details_obj, "description") |> result.map_error(fn(_) { "Missing details.description" }))
      use date <- result.try(json.string_at(details_obj, "date") |> result.map_error(fn(_) { "Missing details.date" }))
      Ok(ClaimCreated(policy_id, Money(amount_val, currency), ClaimDetails(description, date)))
    }
    "ClaimUpdated" -> {
      use details_obj <- result.try(json.object_at(decoded, "details") |> result.map_error(fn(_) { "Missing details" }))
      use description <- result.try(json.string_at(details_obj, "description") |> result.map_error(fn(_) { "Missing details.description" }))
      use date <- result.try(json.string_at(details_obj, "date") |> result.map_error(fn(_) { "Missing details.date" }))
      let amount_opt = case json.object_at(decoded, "amount") {
        Ok(amount_obj) if amount_obj != json.null() -> {
          use amount_val <- result.try(json.float_at(amount_obj, "amount"))
          use currency <- result.try(json.string_at(amount_obj, "currency"))
          Some(Money(amount_val, currency))
        }
        _ -> None
      }
      Ok(ClaimUpdated(ClaimDetails(description, date), amount_opt))
    }
    "ClaimApproved" -> Ok(ClaimApproved)
    "ClaimRejected" -> Ok(ClaimRejected)
    _ -> Error("Unknown event type: " <> event_type)
  }
}