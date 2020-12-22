import { Console } from "console";
import { makeAutoObservable, computed, makeObservable, observable, configure } from "mobx";
import { createContext, SyntheticEvent } from "react";
import agent from "../api/agent";
import { IActivity } from "../models/activity";

//all our state management is done in the activitystore
class ActivityStore {
  activityRegistry = new Map();
  activities: IActivity[] = [];
  selectedActivity: IActivity | undefined;
  loadingInitial = false;
  editMode = false;
  submitting = false;
  target = "";

  constructor() {
    makeAutoObservable(this);
  }

  //we use computed when data is already inside our store but we can work out what the reult should be on already existing data
  //activityRegistery returns an iterable of values in the map and then we get an array of the value and sort by date
  get activitiesByDate() {
    return Array.from(this.activityRegistry.values()).sort(
      (a, b) => Date.parse(a.date) - Date.parse(b.date)
    );
  }

//await is equivalent to then and is actually another function that is not covered by action
  loadActivities = async () => {
    this.loadingInitial = true;
    try {
      const activities = await agent.Activities.list();
      activities.forEach((activity) => {
        activity.date = activity.date.split(".")[0];
        this.activityRegistry.set(activity.id, activity);
      });
      this.loadingInitial = false;
    } catch (error) {
      this.loadingInitial = false;
      console.log(error);
    }
  };
  createActivity = async (activity: IActivity) => {
    this.submitting = true;
    try {
      await agent.Activities.create(activity);
      this.activityRegistry.set(activity.id, activity);
      this.editMode = false;
      this.submitting = false;
    } catch (error) {
      this.submitting = false;
      console.log(error);
    }
  };

  //use set to filter out all the other activities and updated activity back into array
  //submitting is set to true initially but while being edited set to false so only submitted when false
  editActivity = async (activity: IActivity) => {
    this.submitting = true;
    try {
      await agent.Activities.update(activity);
      this.activityRegistry.set(activity.id, activity);
      this.selectedActivity = activity;
      this.editMode = false;
      this.submitting = false;
    } catch (error) {
      this.submitting = false;
      console.log(error);
    }
  };

  deleteActivity = async (
    event: SyntheticEvent<HTMLButtonElement>,
    id: string
  ) => {
    this.submitting = true;
    this.target = event.currentTarget.name;
    try {
      await agent.Activities.delete(id);
      this.activityRegistry.delete(id);
      this.submitting = false;
      this.target = '';
    } catch (error) {
      this.submitting = false;
      this.target = '';
      console.log(error);
    }
  };

  openCreateForm = () => {
    this.editMode = true;
    this.selectedActivity = undefined;
  };

  openEditForm = (id: string) => {
    this.selectedActivity = this.activityRegistry.get(id);
    this.editMode = true;
  };

  cancelSelectedActivity = () => {
    this.selectedActivity = undefined;
  };

  cancelFormOpen = () => {
    this.editMode = false;
  };

  selectActivity = (id: string) => {
    this.selectedActivity = this.activityRegistry.get(id);
    this.editMode = false;
  };
}

export default createContext(new ActivityStore());
