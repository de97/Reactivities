import React, { useEffect, Fragment, useContext } from "react";
import { Container } from "semantic-ui-react";
import "semantic-ui-css/semantic.min.css";
import NavBar from "../../features/nav/NavBar";
import ActivityDashboard from "../../features/activities/dashboard/ActivityDashboard";
import agent from "../api/agent";
import LoadingComponent from "./LoadingComponent";
import ActivityStore from "../stores/activityStore";
import { observer } from "mobx-react-lite";

const App = () => {
  const activityStore = useContext(ActivityStore);

  useEffect(() => {
    activityStore.loadActivities();
  }, [activityStore]);

  if (activityStore.loadingInitial)
    return <LoadingComponent content="Loading activities..." />;

  return (
    <Fragment>
      <NavBar />
      <Container style={{ marginTop: "7em" }}>
        <ActivityDashboard />
      </Container>
    </Fragment>
  );
};

export default observer(App);

//use state management system like mobx means we can lift all o our state outside of components and put into centralised state store
//

//takes an id an sets a selected activity and uses the filter method to retriev the individual activity
//pass it down to activity dashboard

//set activities spread operator activities into a new array

//contain a new array of all the activities that do not match the id of the activity we are passing in

//get the activities and set the state of activities

//[] empty array stops method from running more then once
