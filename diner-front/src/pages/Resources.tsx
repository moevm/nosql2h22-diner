import React from 'react'
import { Input, DatePicker, Button, List, Avatar, Space } from 'antd';
import { Link } from 'react-router-dom';

export const Resources: React.FC = () => {
  const resourcesList = [{
    imageUrl: "https://joeschmoe.io/api/v1/random",
    id: '123',
    name: 'Meet',
    amount: 300
  }, {
    imageUrl: "https://joeschmoe.io/api/v1/random",
    id: '456',
    name: 'Yuri',
    amount: 300
  }]

  return <div>
    <Input.Group compact size="large">
      <Input style={{ width: 'calc(100% - 350px)' }} placeholder="Resources search" />
      <DatePicker.RangePicker style={{ width: '250px' }} size="large" />
      <Button type="primary" style={{ width: '100px' }} size="large">Search</Button>
    </Input.Group>
    <br />
    <br />
    <Space>
      <Link to="/dashboard/resources/add"><Button>Add resource</Button></Link>
      <Button>Import</Button>
      <Button>Export</Button>
    </Space>
    <br />
    <br />
    <List
      size="large"
      header={<div>Resources found: {resourcesList.length}</div>}
      dataSource={resourcesList}
      renderItem={item => <Link key={item.id} to={`/dashboard/resources/resource/${item.id}`}> <List.Item ><List.Item.Meta
        avatar={<Avatar src={item.imageUrl} />}
        title={item.name}
        description={`#${item.id}, ${item.amount} kg`}
      /></List.Item></Link>}
    />
  </div>
}