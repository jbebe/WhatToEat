import { DeliveryServiceType } from "./DeliveryServiceType"
import { z } from "zod";

export const RestaurantSchema = z.object({
    // required field, restaurant name
    name: z.string(),

    // URL to the restaurant
    url: z.string().optional(),

    // if delivery is avaiable, list possible
    delivery: z.record(z.nativeEnum(DeliveryServiceType), z.string().url().optional())
})

export type Restaurant = z.infer<typeof RestaurantSchema>
