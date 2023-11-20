import styles from './App.module.scss'
import RestaurantList from '../RestaurantList/RestaurantList'
import Results from '../Results/Results'
import Presence from '../Presence/Presence'
import Statistics from '../Statistics/Statistics'

export default function App() {
  return (
    <div className={styles.container}>
      <div>Hello Bálint! Today is wednesday.</div>
      <div className={styles.content}>
        <RestaurantList />
        <div>
          <Results />
          <Presence />
          <Statistics />
        </div>
      </div>
    </div>
  )
}
