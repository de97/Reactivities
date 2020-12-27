import { Console } from "console";
import { makeAutoObservable } from "mobx";
import { createContext, SyntheticEvent } from "react";
import agent from "../api/agent";
import { IActivity } from "../models/activity";

//all our state management is done in the activitystore
class ActivityStore {
  activityRegistry = new Map();
  activity: IActivity | null = null; //initially set to null
  loadingInitial = false;
  submitting = false;
  target = "";

  constructor() {
    makeAutoObservable(this);
  }

  //we use computed when data is already inside our store but we can work out what the reult should be on already existing data
  //activityRegistery returns an iterable of values in the map and then we get an array of the value and sort by date
  get activitiesByDate() {
    return this.groupActivitiesByDate(
      Array.from(this.activityRegistry.values())
    );
  }

  //take activities as the parameter

  groupActivitiesByDate(activities: IActivity[]) {
    const sortedActivities = activities.sort(
      (a, b) => Date.parse(a.date) - Date.parse(b.date)
    );
    //reduce our current array that we get from object entries and create a new array of objects where the key of object is the activity date as a string
    //the value is going to be an array of activities
    return Object.entries(
      sortedActivities.reduce((activities, activity) => {
        //just want the date and not the time so take the first element of the array that this produces
        const date = activity.date.split("T")[0];
        //check and compare date if it is matching add to array if true spraed the array of activities for this date and add activity into it if not just add activity
        activities[date] = activities[date]
          ? [...activities[date], activity]
          : [activity];
        return activities;
      }, {} as { [key: string]: IActivity[] })
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

  loadActivity = async (id: string) => {
    let activity = this.getActivity(id);
    if (activity) {
      this.activity = activity;
    } else {
      this.loadingInitial = true;
      try {
        activity = await agent.Activities.details(id);
        this.activity = activity;
        this.loadingInitial = false;
      } catch (error) {
        this.loadingInitial = false;
        console.log(error);
      }
    }
  };

  clearActivity = () => {
    this.activity = null;
  };

  getActivity = (id: string) => {
    return this.activityRegistry.get(id);
  };

  createActivity = async (activity: IActivity) => {
    this.submitting = true;
    try {
      await agent.Activities.create(activity);
      this.activityRegistry.set(activity.id, activity);

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
      this.activity = activity;

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
      this.target = "";
    } catch (error) {
      this.submitting = false;
      this.target = "";
      console.log(error);
    }
  };
}

export default createContext(new ActivityStore());
