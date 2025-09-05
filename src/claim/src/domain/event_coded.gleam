import domain/value_object.{
  type ClaimDetails, type ClaimStatus, type Money, Approved, ClaimDetails, Money,
  Pending, Rejected,
}
import gleam/dynamic/decode
import gleam/json

// Codec for Money
pub fn money_to_json(money: Money) -> String {
  json.object([
    #("amount", json.float(money.amount)),
    #("currency", json.string(money.currency)),
  ])
  |> json.to_string
}

pub fn money_from_json(json_string: String) -> Result(Money, json.DecodeError) {
  let money_decoder = {
    use amount <- decode.field("amount", decode.float)
    use currency <- decode.field("cuncurrency", decode.string)
    decode.success(Money(amount, currency))
  }
  json.parse(json_string, money_decoder)
}

// Codec for ClaimDetails
pub fn claim_details_to_json(claim_details: ClaimDetails) -> String {
  json.object([
    #("description", json.string(claim_details.description)),
    #("date", json.string(claim_details.date)),
  ])
  |> json.to_string
}

pub fn claim_details_from_json(
  json_string: String,
) -> Result(ClaimDetails, json.DecodeError) {
  let money_decoder = {
    use description <- decode.field("description", decode.string)
    use date <- decode.field("date", decode.string)
    decode.success(ClaimDetails(description, date))
  }
  json.parse(json_string, money_decoder)
}

// Coded for ClaimStatus
/// Format in JSON
/// {
///   "claim_status": "Approved"
/// }
pub fn claim_status_to_json(status: ClaimStatus) -> String {
  let status_str = case status {
    Pending -> "Pending"
    Approved -> "Approved"
    Rejected -> "Rejected"
  }
  json.object([#("claim_status", json.string(status_str))])
  |> json.to_string
}

pub fn claim_status_from_json(
  json_string: String,
) -> Result(ClaimStatus, String) {
  case json.parse(json_string, decode.string) {
    Ok("Pending") -> Ok(Pending)
    Ok("Approved") -> Ok(Approved)
    Ok("Rejected") -> Ok(Rejected)
    Ok(_) -> Error("Invalid status value")
    Error(_) -> Error("Failed to decode JSON")
  }
}
