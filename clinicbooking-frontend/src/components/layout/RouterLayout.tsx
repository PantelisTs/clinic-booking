import { Outlet } from "react-router"
import Header from "@/components/layout/Header.tsx"
import Footer from "@/components/layout/Footer.tsx"

export default function RouterLayout() {
    return (
        <div className="flex flex-col min-h-screen bg-muted/60">
            <Header />
            <main className="flex-1 flex flex-col">
                <Outlet />
            </main>
            <Footer />
        </div>
    )
}