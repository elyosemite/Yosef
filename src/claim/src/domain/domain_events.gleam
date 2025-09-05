import domain/value_object.{type ClaimDetails, type Money}
import gleam/option.{type Option}

pub type Event {
  ClaimCreated(policy_id: String, amount: Money, details: ClaimDetails)
  ClaimUpdated(details: ClaimDetails, amount: Option(Money))
  ClaimApproved(policy_id: String)
  ClaimRejected(policy_id: String)
}
