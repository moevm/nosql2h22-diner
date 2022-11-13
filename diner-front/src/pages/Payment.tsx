import React from 'react'
import { Card } from 'antd';
import { Link, useParams } from 'react-router-dom';
import { ArrowLeftOutlined } from '@ant-design/icons';

export const Payment: React.FC = () => {
  const { id } = useParams();
  const paymentData = {
    id,
    name: 'For Yuri <3',
    amount: 300,
    status: 'done'
  }

  return <div>
    <Link to="/dashboard/payments"> Go back to payments</Link>
    <br />
    <Card title={`Payment #${id}`} style={{ width: 501 }}>
      <Card.Grid style={{ width: 250 }}>Name</Card.Grid>
      <Card.Grid style={{ width: 250 }}>{paymentData.name}</Card.Grid>
      <Card.Grid style={{ width: 250 }}>Amount</Card.Grid>
      <Card.Grid style={{ width: 250 }}>$ {paymentData.amount}</Card.Grid>
      <Card.Grid style={{ width: 250 }}>Status</Card.Grid>
      <Card.Grid style={{ width: 250 }}>{paymentData.status}</Card.Grid>
    </Card>
  </div>
}