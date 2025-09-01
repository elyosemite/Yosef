import app/web.{type Context}
import gleam/string_tree
import wisp.{type Request, type Response}

pub fn handle_request(_req: Request, _ctx: Context) -> Response {
  wisp.html_response(string_tree.from_string("Hello, World!"), 200)
}
