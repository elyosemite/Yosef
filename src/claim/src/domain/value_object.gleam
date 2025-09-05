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

pub type ClaimStatus {
  Pending
  Approved
  Rejected
}
