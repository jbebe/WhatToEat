import styles from './App.module.scss'
import RestaurantList from '../RestaurantList/RestaurantList'
import Results from '../Results/Results'
import Presence from '../Presence/Presence'
import Statistics from '../Statistics/Statistics'
import { QueryClient, QueryClientProvider } from 'react-query'
import RestaurantModal from '../RestaurantModal/RestaurantModal'

const queryClient = new QueryClient()

export default function App() {

  return (
    <QueryClientProvider client={queryClient}>
      <div className={styles.container}>
        <div>Hello Bálint! Today is wednesday.</div>
        <div className={styles.content}>
          <RestaurantModal />
          <RestaurantList />
          <div>
            <Results />
            <Presence />
            <Statistics />
          </div>
        </div>
      </div>
    </QueryClientProvider>
  )
}
