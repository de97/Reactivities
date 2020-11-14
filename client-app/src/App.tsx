import React, { Component } from "react";
import { Header, Icon, List } from "semantic-ui-react";
import "semantic-ui-css/semantic.min.css";
import "./App.css";
import axios from "axios";

class App extends Component {
  state = {
    values: [],
  };

  componentDidMount() {
    //get data from server
    axios.get("http://localhost:5000/api/values").then((response) => {
      this.setState({
        values: response.data,
      });
    });
    this.setState({
      values: [
        { id: 1, name: "Value 101" },
        { id: 2, name: "Value 102" },
      ],
    });
  }

  render() {
    return (
      <div>
        <Header as="h2">
          <Icon name="users" />
          <Header.Content>Reactivities</Header.Content>
        </Header>
        <List>
          {this.state.values.map((value: any) => (
            //always give key value so it is unique
            <List.Item key={value.id}>{value.name}</List.Item>
          ))}
        </List>
      </div>
    );
  }
}

export default App;
