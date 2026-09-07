import { Input } from "@/components/ui/input.tsx"
import { Label } from "@/components/ui/label.tsx"
import { Button } from "@/components/ui/button.tsx"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card.tsx"
import { useForm } from "react-hook-form"
import { type LoginFields, loginSchema } from "@/schemas/auth.ts"
import { zodResolver } from "@hookform/resolvers/zod"
import { useAuth } from "@/context/AuthProvider.tsx"
import { toast } from "sonner"
import { useNavigate } from "react-router"

export default function LoginPage() {
    const { loginUser } = useAuth()
    const navigate = useNavigate()

    const {
        register,
        handleSubmit,
        formState: { errors, isSubmitting },
    } = useForm<LoginFields>({
        resolver: zodResolver(loginSchema),
        defaultValues: { keepLoggedIn: false },
    })

    const onSubmit = async (data: LoginFields) => {
        try {
            await loginUser(data)
            toast.success("Login successful")
            navigate("/dashboard")
        } catch (error) {
            toast.error(error instanceof Error ? error.message : "Login failed")
        }
    }

    return (
        <div className="flex items-center justify-center min-h-screen">
            <Card className="w-full max-w-sm">
                <CardHeader>
                    <CardTitle>Login</CardTitle>
                </CardHeader>
                <CardContent>
                    <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
                        <div className="space-y-2">
                            <Label htmlFor="username">Username</Label>
                            <Input id="username" {...register("username")} />
                            {errors.username && (
                                <p className="text-sm text-destructive">{errors.username.message}</p>
                            )}
                        </div>
                        <div className="space-y-2">
                            <Label htmlFor="password">Password</Label>
                            <Input id="password" type="password" {...register("password")} />
                            {errors.password && (
                                <p className="text-sm text-destructive">{errors.password.message}</p>
                            )}
                        </div>
                        <div className="flex items-center gap-2">
                            <input id="keepLoggedIn" type="checkbox" {...register("keepLoggedIn")} />
                            <Label htmlFor="keepLoggedIn">Keep me logged in</Label>
                        </div>
                        <Button type="submit" className="w-full" disabled={isSubmitting}>
                            {isSubmitting ? "Logging in..." : "Login"}
                        </Button>
                    </form>
                </CardContent>
            </Card>
        </div>
    )
}