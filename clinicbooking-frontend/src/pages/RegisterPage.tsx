import { useState } from "react"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card.tsx"
import { Button } from "@/components/ui/button.tsx"
import DoctorSignupForm from "@/components/auth/DoctorSignupForm.tsx"
import PatientSignupForm from "@/components/auth/PatientSignupForm.tsx"
import {Link} from "react-router";

type AccountType = "doctor" | "patient"

export default function RegisterPage() {
    const [accountType, setAccountType] = useState<AccountType>("patient")

    return (
        <div className="flex-1 flex items-center justify-center">
            <Card className="w-full max-w-sm">
                <CardHeader>
                    <CardTitle>Create account</CardTitle>
                    <div className="flex gap-2 pt-2">
                        <Button
                            type="button"
                            variant={accountType === "patient" ? "default" : "outline"}
                            className="flex-1"
                            onClick={() => setAccountType("patient")}
                        >
                            Patient
                        </Button>
                        <Button
                            type="button"
                            variant={accountType === "doctor" ? "default" : "outline"}
                            className="flex-1"
                            onClick={() => setAccountType("doctor")}
                        >
                            Doctor
                        </Button>
                    </div>
                </CardHeader>
                <CardContent>
                    {accountType === "patient" ? <PatientSignupForm /> : <DoctorSignupForm />}
                    <p className="text-sm text-center text-muted-foreground mt-4">
                        Already have an account?{" "}
                        <Link to="/login" className="underline underline-offset-4">
                            Log in
                        </Link>
                    </p>
                </CardContent>
            </Card>

        </div>
    )
}