import React, { Fragment } from "react";
import { Container } from "semantic-ui-react";
import "semantic-ui-css/semantic.min.css";
import NavBar from "../../features/nav/NavBar";
import ActivityDashboard from "../../features/activities/dashboard/ActivityDashboard";
import { observer } from "mobx-react-lite";
import { Route, RouteComponentProps, withRouter } from "react-router-dom";
import HomePage from "../../features/home/HomePage";
import ActivityForm from "../../features/activities/form/ActivityForm";
import ActivityDetails from "../../features/activities/details/ActivityDetails";

const App: React.FC<RouteComponentProps> = ({ location }) => {
  return (
    <Fragment>
      <Route exact path="/" component={HomePage} />
      {/* when are hitting a route with forward slash and anything else this is the route it will match otherwise match homepage*/}
      <Route
        path={"/(.+)"}
        render={() => (
          <Fragment>
            <NavBar />
            <Container style={{ marginTop: "7em" }}>
              <Route exact path="/activities" component={ActivityDashboard} />
              <Route path="/activities/:id" component={ActivityDetails} />
              {/* add key to this  route so that when props change inside route it creates a new instance of whatever component is loading e.g. click on create actiity from edit form*/}
              <Route
                key={location.key}
                path={["/createActivity", "/manage/:id"]}
                component={ActivityForm}
              />
            </Container>
          </Fragment>
        )}
      />
    </Fragment>
  );
};

export default withRouter(observer(App));

//use state management system like mobx means we can lift all o our state outside of components and put into centralised state store
//

//takes an id an sets a selected activity and uses the filter method to retriev the individual activity
//pass it down to activity dashboard

//set activities spread operator activities into a new array

//contain a new array of all the activities that do not match the id of the activity we are passing in

//get the activities and set the state of activities

//[] empty array stops method from running more then once
