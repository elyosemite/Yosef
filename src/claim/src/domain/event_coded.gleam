import domain/claim.{
  type ClaimDetails, type Money, ClaimApproved, ClaimCreated, ClaimDetails,
  ClaimRejected, ClaimUpdated, Money,
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
