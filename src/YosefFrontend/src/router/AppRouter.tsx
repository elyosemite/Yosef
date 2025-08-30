import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import Users from "../pages/Users";
import Page from "@/dashboard/page";

export default function AppRouter() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Page />} />

        <Route path="/users" element={<Users />} />

        <Route path="*" element={<Navigate to="/" />} />
      </Routes>
    </BrowserRouter>
  );
}
