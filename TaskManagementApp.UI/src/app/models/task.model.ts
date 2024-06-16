export interface Task {
    id: string;
    title: string;
    description: string;
    dueDate: string;
    priority: 'high' | 'medium' | 'low';
    completed: boolean;
    showDetails?: boolean; // Optional property to toggle task details visibility
  }
  