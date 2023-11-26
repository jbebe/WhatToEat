export enum DeliveryServiceType {
  Wolt = 'wolt',
  Foodpanda = 'foodpanda'
}

export type Restaurant = {
  name: string
  url?: string
  delivery?: Record<DeliveryServiceType, string>
  distance?: number
}