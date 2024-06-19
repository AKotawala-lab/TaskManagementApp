import { User } from "./user.model";

export enum TaskPriority {
  Low = 1,
  Medium = 2,
  High = 3
}

export interface Task {
    id: string;
    title: string;
    description?: string;
    createdAt: string;
    dueDate: string;
    reminder?: string;
    priority: TaskPriority;
    userId: string;
    completed: boolean;
    showDetails?: boolean; // Optional property to toggle task details visibility

    user?: User;
  }
  