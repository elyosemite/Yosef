import app/web.{type Context}
import gleam/dynamic/decode
import gleam/http.{Post}
import gleam/json
import gleam/result
import gleam/string_tree.{from_string}
import wisp.{type Request, type Response}

pub type Claim {
  Claim(policy_id: String, claim_cause: String)
}

fn claim_decoder() -> decode.Decoder(Claim) {
  use policy_id <- decode.field("policy_id", decode.string)
  use claim_cause <- decode.field("claim_cause", decode.string)
  decode.success(Claim(policy_id, claim_cause))
}

pub fn handle_request(req: Request, ctx: Context) -> Response {
  use req <- web.middleware(req, ctx)
  use <- wisp.require_method(req, Post)

  use json <- wisp.require_json(req)

  let result = {
    use claim <- result.try(decode.run(json, claim_decoder()))

    let object =
      json.object([
        #("policy_id", json.string(claim.policy_id)),
        #("claim_cause", json.string(claim.claim_cause)),
      ])
    Ok(json.to_string(object))
  }

  case wisp.path_segments(req) {
    [] -> {
      wisp.html_response(string_tree.from_string("Home"), 200)
    }
    ["claim"] -> {
      case result {
        Ok(json) -> wisp.json_response(from_string(json), 201)
        Error(_) -> wisp.unprocessable_entity()
      }
    }
    ["internal-server-error"] -> wisp.internal_server_error()
    ["unprocessable-entity"] -> wisp.unprocessable_entity()
    ["method-not-allowed"] -> wisp.method_not_allowed([])
    ["entity-too-large"] -> wisp.entity_too_large()
    ["bad-request"] -> wisp.bad_request()
    _ -> wisp.not_found()
  }
  // wisp.html_response(string_tree.from_string("Hello, World!"), 200)
}
