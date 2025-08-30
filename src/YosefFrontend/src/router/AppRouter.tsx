import { BrowserRouter, Routes, Route } from "react-router-dom";
import Users from "../pages/Users";

export default function AppRouter() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/users" element={<Users />} />
      </Routes>
    </BrowserRouter>
  );
}
