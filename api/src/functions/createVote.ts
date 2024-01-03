import { app, HttpRequest, HttpResponseInit, InvocationContext } from "@azure/functions";
import { TableOutput } from "../utils/table"
import { VoteRequestSchema } from "what-to-eat";
import { createVoteEntity } from "../entities/VoteEntity";

app.http('createVote', {
    trigger: {
        authLevel: 'anonymous',
        methods: ['POST'],
        route: 'vote',
        name: 'createVote',
        type: 'httpTrigger',
    },
    async handler(request: HttpRequest, context: InvocationContext): Promise<HttpResponseInit> {
        const userId = request.headers.get('x-wte-userid')
        const req = VoteRequestSchema.parse(await request.json())
        const entity = createVoteEntity(userId, req)
        context.extraOutputs.set(TableOutput, entity)
        return { jsonBody: entity.ClientData }
    },
    extraOutputs: [TableOutput]
});
