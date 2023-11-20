import React from "react"
import styles from './Card.module.scss'

type Props = {
  title: string
  className?: string
  children: React.ReactNode | React.ReactNode[]
}

export default function Card({ children, className, title }: Props) {
  return (
    <div className={`${className} ${styles.container}`}>
      <h4>{title}</h4>
      {children}
    </div>
  )
}
