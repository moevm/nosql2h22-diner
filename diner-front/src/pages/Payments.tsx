import React from 'react'
import { Input, DatePicker, Button, List } from 'antd';
import { Link } from 'react-router-dom';

export const Payments: React.FC = () => {
  const paymentsList = [{
    id: '123',
    name: 'For Yuri <3',
    amount: 300
  }, {
    id: '456',
    name: 'For Yuri again <3',
    amount: 300
  }]

  return <div>
    <Input.Group compact size="large">
      <Input style={{ width: 'calc(100% - 350px)' }} placeholder="Payments search" />
      <DatePicker.RangePicker style={{ width: '250px' }} size="large" />
      <Button type="primary" style={{ width: '100px' }} size="large">Search</Button>
    </Input.Group>
    <br />
    <br />
    <List
      size="large"
      header={<div>Payments found: {paymentsList.length}</div>}
      dataSource={paymentsList}
      renderItem={item => <Link key={item.id} to={`/dashboard/payments/payment/${item.id}`}> <List.Item >#{item.id} {item.name} <strong>$ {item.amount}</strong></List.Item></Link>}
    />
  </div>
}