import gleam/float
import gleam/option.{type Option, None, Some}
import gleam/result
import gleam/string

pub type Money {
  Money(value: Float, currency: String)
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

// Commands (CQRS Command side)
pub type Command {
  CreateClaim(policy_id: String, amount: Money, details: ClaimDetails)
  ApproveClaim
  RejectClaim
}

pub type Event {
  ClaimCreated(policy_id: String, amount: Money, details: ClaimDetails)
  ClaimApproved
  ClaimRejected
}

pub fn handle_command(
  state: Option(ClaimState),
  cmd: Command,
) -> Result(List(Event), String) {
  case cmd {
    CreateClaim(policy_id, amount, details) -> {
      // Regra: Não criar se já existe
      case state {
        Some(_) -> Error("Claim already exists")
        None -> Ok([ClaimCreated(policy_id, amount, details)])
      }
    }
    ApproveClaim -> {
      case state {
        Some(s) if s.status == "pending" && s.amount.value <=. 10_000.0 -> {
          Ok([ClaimApproved])
        }
        Some(_) -> Error("Cannot approve: invalid status or amount too high")
        None -> Error("Claim not found")
      }
    }
    RejectClaim -> {
      case state {
        Some(s) if s.status == "pending" -> Ok([ClaimRejected])
        Some(_) -> Error("Cannot reject: invalid status")
        None -> Error("Claim not found")
      }
    }
  }
}
