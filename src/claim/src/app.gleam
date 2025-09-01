import gleam/erlang/process
import mist
import wisp
import wisp/wisp_mist

pub fn main() {
  wisp.configure_logger()

  let assert Ok(_) =
    wisp_mist.handler(fn(_) { todo }, "secret_key")
    |> mist.new
    |> mist.port(8000)
    |> mist.start

  process.sleep_forever()
}
