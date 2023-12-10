import { app, input, output, HttpRequest, HttpResponseInit, InvocationContext } from "@azure/functions";
import { Restaurant, RestaurantSchema } from "what-to-eat";

const tableOutput = output.table({
    connection: 'MyStorageConnectionAppSetting',
    tableName: 'whattoeat',
    partitionKey: 'products',
    rowKey: '{id}',
});

export async function createRestaurant(request: HttpRequest, context: InvocationContext): Promise<HttpResponseInit> {
    const restaurant = RestaurantSchema.parse(await request.json() as Restaurant);
    const output = context.extraOutputs.get(tableOutput)
    
    const name = request.query.get('name') || await request.text() || 'world';

    return { body: `Hello, ${name}!` };
};

app.http('restaurant', {
    methods: ['POST'],
    authLevel: 'anonymous',
    handler: createRestaurant,
    extraOutputs: [tableOutput]
});
