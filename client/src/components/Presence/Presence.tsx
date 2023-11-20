import { useState } from "react"
import Card from "../../shared/Card/Card"

export default function Results() {
  const [count, setCount] = useState(0)

  return (
    <Card title="Online users">
      <ul>
        <li>Máté</li>
        <li>Ádám</li>
        <li>Gergő</li>
      </ul>
    </Card>
  )
}
