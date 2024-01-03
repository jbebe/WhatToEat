import { app, HttpRequest, InvocationContext } from "@azure/functions"
import { GetTableInput } from "../utils/table"
import { VoteEntity } from "../entities/VoteEntity"

const tableInput = GetTableInput('vote', { 
    filter: "(RowKey ge '{date}' and RowKey lt '{date}z')"
 })

app.http('listVotes', {
    trigger: {
        authLevel: 'anonymous',
        methods: ['GET'],
        route: 'vote/{date}',
        name: 'listVotes',
        type: 'httpTrigger',
    },
    handler(request: HttpRequest, context: InvocationContext)
    {
        const votes = (context.extraInputs.get(tableInput) as VoteEntity[])
            .map(x => JSON.parse(x.ClientData as any))
        return ({ jsonBody: votes })
    },
    extraInputs: [tableInput]
})
