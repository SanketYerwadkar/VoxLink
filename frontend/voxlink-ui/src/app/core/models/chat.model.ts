import { User } from './user.model';

export interface Message {
  id: string;
  userId: string;
  userName: string;
  content: string;
  timestamp: Date;
  type: 'text' | 'image' | 'file';
}

export interface ChatRoom {
  id: string;
  name: string;
  description?: string;
  participants: User[];
  lastMessage?: Message;
  createdAt: Date;
  isGroup: boolean;
}

export interface SendMessageRequest {
  content: string;
  type: 'text' | 'image' | 'file';
  roomId?: string;
  recipientId?: string;
}
