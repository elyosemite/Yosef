import domain/domain_events.{
  type Event, ClaimApproved, ClaimCreated, ClaimRejected, ClaimUpdated,
}
import domain/value_object.{
  type ClaimDetails, type ClaimStatus, type Money, Approved, Pending, Rejected,
}
import gleam/list
import gleam/option.{type Option, None, Some}

pub type ClaimState {
  ClaimState(
    id: String,
    policy_id: String,
    amount: Money,
    status: ClaimStatus,
    details: ClaimDetails,
    version: Int,
  )
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
            status: Pending,
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
        ClaimApproved(policy_id) -> {
          let assert Some(s) = acc
          Some(
            ClaimState(
              ..s,
              policy_id: policy_id,
              status: Approved,
              version: s.version + 1,
            ),
          )
        }
        ClaimRejected(policy_id) -> {
          let assert Some(s) = acc
          Some(
            ClaimState(
              ..s,
              policy_id: policy_id,
              status: Rejected,
              version: s.version + 1,
            ),
          )
        }
      }
    })
  case final_state {
    Some(state) -> Ok(state)
    None -> Error("No events to build state")
  }
}
