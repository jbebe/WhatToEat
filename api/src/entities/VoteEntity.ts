import { Vote, VoteRequest } from "what-to-eat"
import { BaseEntity } from "../utils/table"

export interface VoteEntity extends BaseEntity<Vote> {
    PartitionKey: 'vote',
}

export function createVoteEntity(userId: string, request: VoteRequest): VoteEntity {
    const date = new Date().toJSON().split('T')[0].replace(/-/g, '');
    return {
        PartitionKey: 'vote',
        RowKey: `${date}_${userId}_${request.restaurantId}`,
        ClientData: {
            date: date,
            userId: userId,
            ...request,
        },
    }
}
