import gleam/list
import gleam/option.{type Option, None, Some}

pub type Money {
  Money(amount: Float, currency: String)
}

pub fn new_money(amount: Float, currency: String) -> Result(Money, String) {
  case amount >. 0.0 {
    True -> Ok(Money(amount, currency))
    False -> Error("Amount must be positive")
  }
}

pub type ClaimDetails {
  ClaimDetails(description: String, date: String)
}

pub type ClaimState {
  ClaimState(
    id: String,
    policy_id: String,
    amount: Money,
    // "pending", "approved", "rejected"
    status: String,
    details: ClaimDetails,
    version: Int,
  )
}

pub type Event {
  ClaimCreated(policy_id: String, amount: Money, details: ClaimDetails)
  ClaimUpdated(details: ClaimDetails, amount: Option(Money))
  ClaimApproved
  ClaimRejected
}

pub fn apply_events(events: List(Event)) -> Result(ClaimState, String) {
  let initial = None
  let final_state =
    list.fold(events, initial, fn(acc, event) {
      case event {
        ClaimCreated(policy_id, amount, details) -> {
          Some(ClaimState(
            id: "",
            policy_id: policy_id,
            amount: amount,
            status: "pending",
            details: details,
            version: 1,
          ))
        }
        ClaimUpdated(new_details, new_amount) -> {
          let assert Some(s): Option(ClaimState) = acc
          let updated_amount = option.unwrap(new_amount, s.amount)
          Some(
            ClaimState(
              ..s,
              details: new_details,
              amount: updated_amount,
              version: s.version + 1,
            ),
          )
        }
        ClaimApproved -> {
          let assert Some(s) = acc
          Some(ClaimState(..s, status: "approved", version: s.version + 1))
        }
        ClaimRejected -> {
          let assert Some(s) = acc
          Some(ClaimState(..s, status: "rejected", version: s.version + 1))
        }
      }
    })
  case final_state {
    Some(state) -> Ok(state)
    None -> Error("No events to build state")
  }
}
