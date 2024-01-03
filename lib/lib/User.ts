import { z } from "zod"

export const UserRequestSchema = z.object({
    name: z.string(),
})

export type UserRequest = z.infer<typeof UserRequestSchema>

export type User = UserRequest & { id: string }
