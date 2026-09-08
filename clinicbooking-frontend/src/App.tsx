import { Route, Routes } from "react-router"
import LoginPage from "@/pages/LoginPage.tsx"
import Dashboard from "@/pages/Dashboard.tsx"
import ProtectedRoute from "@/components/auth/ProtectedRoute.tsx"
import RegisterPage from "@/pages/RegisterPage.tsx";

function App() {
    return (
        <Routes>
            <Route path="login" element={<LoginPage />} />

            <Route path="register" element={<RegisterPage />} />

            <Route element={<ProtectedRoute />}>
                <Route path="dashboard" element={<Dashboard />} />
            </Route>
        </Routes>
    )
}

export default App