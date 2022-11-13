import React from 'react'
import { Card } from 'antd';
import { Link, useParams } from 'react-router-dom';
import { ArrowLeftOutlined } from '@ant-design/icons';
import { Image, Input, InputNumber } from 'antd';
import { Comments } from './Comments';

export const Resource: React.FC = () => {
  const { id } = useParams();
  const resourceData = {
    imageUrl: "https://joeschmoe.io/api/v1/random",
    id,
    name: 'Meet',
    amount: 300
  };

  const [name, setName] = React.useState(resourceData.name);
  const [amount, setAmount] = React.useState<number | null>(resourceData.amount);

  return <div>
    <Link to="/dashboard/resources"> <ArrowLeftOutlined /> Go back to resources</Link>
    <br />
    <br />
    <Image
      width={200}
      src={resourceData.imageUrl}
    />
    <br />
    <br />
    <Card title={`Resource #${id}`} style={{ width: 501 }}>
      <Card.Grid style={{ width: 250 }}>Name</Card.Grid>
      <Card.Grid style={{ width: 250 }}><Input value={name} onChange={(event) => setName(event.target.value)} /></Card.Grid>
      <Card.Grid style={{ width: 250 }}>Amount</Card.Grid>
      <Card.Grid style={{ width: 250 }}><InputNumber value={amount} onChange={(setAmount)} /> kg</Card.Grid>
    </Card>
    <br />
    <br />
    <Comments entity='resource' entityId={id!} />
  </div>
}