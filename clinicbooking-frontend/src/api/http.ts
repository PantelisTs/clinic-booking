export const API_URL = import.meta.env.VITE_API_URL

type HttpMethod = "GET" | "POST" | "PUT" | "PATCH"

type RequestOptions = {
    token?: string | null
    body?: unknown
}

export class UnauthorizedError extends Error {}

function extractErrorMessage(data: unknown): string {
    if (data && typeof data === "object") {
        if ("detail" in data && typeof data.detail === "string") {
            return data.detail === "Bad Credentials"
                ? "Incorrect username or password"
                : data.detail
        }
        if ("errors" in data && data.errors && typeof data.errors === "object") {
            const firstField = Object.values(data.errors)[0]
            if (Array.isArray(firstField) && typeof firstField[0] === "string") {
                const message = firstField[0]
                return message.startsWith("Password must contain")
                    ? "Password must be 8+ characters with upper/lowercase, a number and a symbol"
                    : message
            }
        }
    }
    return "Request failed"
}

export async function request<T>(
    method: HttpMethod,
    path: string,
    { token, body }: RequestOptions = {}
): Promise<T> {
    const headers: Record<string, string> = {}
    if (body !== undefined) headers["Content-Type"] = "application/json"
    if (token) headers["Authorization"] = `Bearer ${token}`

    const res = await fetch(API_URL + path, {
        method,
        headers,
        body: body !== undefined ? JSON.stringify(body) : undefined,
    })

    if (res.status === 401 && token) {
        throw new UnauthorizedError("Session expired. Please log in again.")
    }

    if (!res.ok) {
        let message = "Request failed"
        try {
            const data = await res.json()
            message = extractErrorMessage(data)
        } catch (error) {
            console.error("Error parsing error response", error)
        }
        throw new Error(message)
    }

    return await res.json()
}