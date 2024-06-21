import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { TaskService } from '../../services/task.service';
import { AuthenticationService } from '../../services/authentication.service';
import { Task, TaskPriority } from '../../models/task.model';
import { User } from '../../models/user.model';

@Component({
  selector: 'app-add-edit-task',
  templateUrl: './add-edit-task.component.html',
  styleUrls: ['./add-edit-task.component.css']
})
export class AddEditTaskComponent implements OnInit {
  @Input() task: Task = {
    id: '',
    title: '',
    description: '',
    createdAt: new Date().toISOString(),
    dueDate: new Date().toISOString(),
    priority: TaskPriority["Low"],
    userId: '',
    completed: false
  };
  user: User;
  token: string;
  errorMessage: string = '';
  
  taskPriorities = [
    { value: TaskPriority.Low, label: 'Low' },
    { value: TaskPriority.Medium, label: 'Medium' },
    { value: TaskPriority.High, label: 'High' },
  ];

  constructor(
    public activeModal: NgbActiveModal,
    private taskService: TaskService,
    private authService: AuthenticationService
  ) {
    this.user = this.authService.getCurrentUser();
    this.token = this.authService.getToken();
  }

  ngOnInit(): void {
    
    if (!this.task) {
      this.task = {
        id: '',
        title: '',
        description: '',
        createdAt: new Date().toISOString(),
        dueDate: new Date().toISOString(),
        priority: TaskPriority["Low"],
        userId: this.user.id,
        completed: false
      };
    }
  }

  onPriorityChange(value: any) { 
    this.task.priority = +TaskPriority[value.target.selectedOptions[0].text]; 
  } 

  saveTask(): void {
    if (this.task) {
      if (this.task.id) {
        // Update existing task
        this.taskService.updateTask(this.task.id, this.task, this.token).subscribe({
          next: (updatedTask) => {
            this.activeModal.close(updatedTask);
          },
          error: (error) => {
            this.errorMessage = 'Failed to update task. Please try again.';
            console.error('Update task error:', error);
          }
        });
      } else {
        // Create new task
        this.taskService.createTask(this.task, this.token).subscribe({
          next: (newTask) => {
            this.activeModal.close(newTask);
          },
          error: (error) => {
            this.errorMessage = 'Failed to create task. Please try again.';
            console.error('Create task error:', error);
          }
        });
      }
    }
  }

  cancel(): void {
    this.activeModal.dismiss();
  }
}
