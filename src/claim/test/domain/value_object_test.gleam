import domain/value_object.{Money}
import gleeunit/should

pub fn money_test() {
  value_object.new_money(90.0, "USD")
  |> should.equal(Ok(Money(90.0, "USD")))
}
