import { DeliveryServiceType } from "./DeliveryServiceType"
import { z } from "zod"

export const VoteRequestSchema = z.object({
    restaurantId: z.string(),
    method: z.array(z.nativeEnum(DeliveryServiceType)),
})

export type VoteRequest = z.infer<typeof VoteRequestSchema>

export type Vote = VoteRequest & {
    date: string,
    userId: string,
}
