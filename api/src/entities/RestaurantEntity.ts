import { Restaurant, RestaurantRequest } from "what-to-eat"
import { BaseEntity } from "../utils/table"
import { createId } from "../utils/id"

export interface RestaurantEntity extends BaseEntity<Restaurant> {
    PartitionKey: 'restaurant',
}

export function createRestaurantEntity(request: RestaurantRequest): RestaurantEntity {
    const id = createId()
    return {
        PartitionKey: 'restaurant',
        RowKey: id,
        ClientData: {
            id,
            ...request,
        },
    }
}
