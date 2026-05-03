export const APP_CONSTANTS = {
  SIGNALR_EVENTS: {
    RECEIVE_MESSAGE: 'ReceiveMessage',
    USER_CONNECTED: 'UserConnected',
    USER_DISCONNECTED: 'UserDisconnected',
    TYPING: 'Typing',
    STOP_TYPING: 'StopTyping'
  },
  STORAGE_KEYS: {
    CURRENT_USER: 'currentUser',
    THEME: 'theme',
    LANGUAGE: 'language'
  },
  API_ENDPOINTS: {
    AUTH: {
      LOGIN: '/api/auth/login',
      REGISTER: '/api/auth/register',
      REFRESH: '/api/auth/refresh'
    },
    USER: {
      PROFILE: '/api/user/profile',
      ONLINE_USERS: '/api/user/online'
    },
    CHAT: {
      ROOMS: '/api/chat/rooms',
      MESSAGES: '/api/chat/messages',
      SEND_MESSAGE: '/api/chat/send'
    }
  },
  UI_CONSTANTS: {
    MAX_MESSAGE_LENGTH: 1000,
    MAX_FILE_SIZE: 10485760, // 10MB
    SUPPORTED_FILE_TYPES: ['.jpg', '.jpeg', '.png', '.gif', '.pdf', '.doc', '.docx']
  }
};
