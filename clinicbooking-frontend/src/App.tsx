import { Route, Routes } from "react-router"
import LoginPage from "@/pages/LoginPage.tsx"
import Dashboard from "@/pages/Dashboard.tsx"
import ProtectedRoute from "@/components/auth/ProtectedRoute.tsx"
import RegisterPage from "@/pages/RegisterPage.tsx";
import RouterLayout from "@/components/layout/RouterLayout.tsx";

function App() {
    return (
        <Routes>
            <Route element={<RouterLayout />}>
                <Route path="login" element={<LoginPage />} />
                <Route path="register" element={<RegisterPage />} />

                <Route element={<ProtectedRoute />}>
                    <Route path="dashboard" element={<Dashboard />} />
                </Route>
            </Route>
        </Routes>
    )
}

export default App