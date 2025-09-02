import domain/claim.{type Command, type Event, ClaimState, apply, handle_command}
import gleam/json
import gleam/list
import gleam/option.{None, Some}
import gleam/result
import signal

// Encoder/Decoder para events (para JSON storage)
fn encode_event(event: Event) -> String {
  case event {
    ClaimCreated(p, a, d) ->
      json.object([
        #("type", json.string("ClaimCreated")),
        #("policy_id", json.string(p)),
        #(
          "amount",
          json.object([
            #("amount", json.float(a.amount)),
            #("currency", json.string(a.currency)),
          ]),
        ),
        #(
          "details",
          json.object([
            #("description", json.string(d.description)),
            #("date", json.string(d.date)),
          ]),
        ),
      ])
      |> json.to_string
    _ -> todo
    // Similar para outros
  }
}

fn decode_event(name: String, data: String) -> Result(Event, String) {
  // Implementar decoding com json.decode
  todo
}

// Command Handler (CQRS write side)
pub fn handle_claim_command(
  signal: signal.Signal,
  aggregate_id: String,
  cmd: Command,
) -> Result(ClaimState, String) {
  use aggregate <- result.try(
    signal.aggregate(signal, aggregate_id)
    |> result.replace_error("Aggregate not found"),
  )
  let state = signal.get_state(aggregate) |> option.from_result
  use events <- result.try(handle_command(state, cmd))
  use new_state <- result.try(signal.handle_events(aggregate, events))
  // Aplica events
  Ok(new_state)
}

// Query Handler (CQRS read side - rebuild from events)
pub fn get_claim_by_id(
  signal: signal.Signal,
  aggregate_id: String,
) -> Result(ClaimState, String) {
  use aggregate <- result.try(signal.aggregate(signal, aggregate_id))
  let state = signal.get_state(aggregate)
  Ok(state)
}

pub fn list_claims(signal: signal.Signal) -> List(ClaimState) {
  // Para list, precisaria de projection ou scan de aggregates; simplificado
  todo
  // Use projections do Signal para efficiency
}
