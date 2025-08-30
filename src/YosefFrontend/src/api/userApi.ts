import type { User } from "../types/user";

const BASE_URL = "http://localhost:3000";

export async function getUsers(): Promise<User[]> {
  const res = await fetch(`${BASE_URL}/users`);
  if (!res.ok) throw new Error("Error to get users");
  return res.json();
}
