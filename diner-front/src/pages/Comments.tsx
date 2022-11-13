import React from 'react'
import { Avatar, Button, Comment, Input, Space } from 'antd'

export const Comments: React.FC<{
  entity: string,
  entityId: string | number,
}> = ({ entity, entityId }) => {
  const comments = [{
    id: 1,
    authorName: 'Yuri',
    authorPic: "https://joeschmoe.io/api/v1/random",
    text: 'Hello!',
  }, {
    id: 2,
    authorName: 'Yuri',
    authorPic: "https://joeschmoe.io/api/v1/random",
    text: 'Good dishes',
  }, {
    id: 3,
    authorName: 'Yuri',
    authorPic: "https://joeschmoe.io/api/v1/random",
    text: 'Hehe',
  }, {
    id: 4,
    authorName: 'Yuri',
    authorPic: "https://joeschmoe.io/api/v1/random",
    text: 'Woop',
  }];

  const [newComment, setNewComment] = React.useState<string | null>(null);

  return <div>
    {newComment === null ? <Button onClick={() => setNewComment('')}>Add comment</Button> : <div>
      <Space direction='vertical' style={{ width: 500 }}>
        <Input.TextArea value={newComment} onChange={(event) => setNewComment(event.target.value)} />
        <Space>
          <Button onClick={() => setNewComment(null)}>Discard</Button>
          <Button type="primary" disabled={!newComment}>Send comment</Button>
        </Space>
      </Space>
    </div>}
    {comments.map(comment => <Comment
      key={comment.id}
      author={comment.authorName}
      avatar={<Avatar src={comment.authorPic} alt={comment.authorName} />}
      content={
        <p>
          {comment.text}
        </p>
      }
    />)}
  </div>
}