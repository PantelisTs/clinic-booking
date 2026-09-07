import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import App from './App.tsx'
import './index.css'
import { BrowserRouter } from "react-router"
import { AuthProvider } from "@/context/AuthProvider.tsx"
import {Toaster} from "sonner";

createRoot(document.getElementById('root')!).render(
    <StrictMode>
        <AuthProvider>
            <BrowserRouter>
                <App />
                <Toaster richColors position="top-center" />
            </BrowserRouter>
        </AuthProvider>
    </StrictMode>,
)