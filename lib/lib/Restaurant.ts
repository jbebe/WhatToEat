import { DeliveryServiceType } from "./DeliveryServiceType"
import { z } from "zod"

export const RestaurantRequestSchema = z.object({
    // required field, restaurant name
    name: z.string(),

    // URL to the restaurant
    url: z.string().optional(),

    // if delivery is available, list possible delivery services
    delivery: z.record(
        z.nativeEnum(DeliveryServiceType),
        z.string().url().optional()
    ).optional()
})

export type RestaurantRequest = z.infer<typeof RestaurantRequestSchema>

export type Restaurant = RestaurantRequest & { id: string }