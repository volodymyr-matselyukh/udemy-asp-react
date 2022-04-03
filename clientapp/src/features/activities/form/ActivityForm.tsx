import { observer } from "mobx-react-lite";
import React, { ChangeEvent, useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { Button, Form, Segment } from "semantic-ui-react";
import LoadingComponent from "../../../app/layout/LoadingComponents";
import { Activity } from "../../../app/models/activity";
import { useStore } from "../../../app/stores/store";
import { v4 as uuid } from "uuid";
import { useNavigate } from "react-router-dom";
import { Link } from "react-router-dom";

export default observer(function ActivityForm() {
  const { activityStore } = useStore();
  const history = useNavigate();
  const {
    loadingInitial,
    loading,
    updateActivity,
    createActivity,
    loadActivity,
  } = activityStore;
  const { id } = useParams<{ id: string }>();

  const [activity, setActivity] = useState<Activity>({
    id: "",
    title: "",
    category: "",
    description: "",
    date: "",
    city: "",
    venue: "",
  });

  useEffect(() => {
    if (id) {
      loadActivity(id).then((activity) => {
        if (activity) {
          setActivity(activity);
        }
      });
    } else {
      setActivity({
        id: "",
        title: "",
        category: "",
        description: "",
        date: "",
        city: "",
        venue: "",
      });
    }
  }, [id]);

  function handleSubmit() {
    if (activity.id.length === 0) {
      let newActivity = {
        ...activity,
        id: uuid(),
      };
      createActivity(newActivity).then(() => {
        history(`/activities/${newActivity.id}`);
      });
    } else {
      updateActivity(activity).then(() => {
        history(`/activities/${activity.id}`);
      });
    }
  }

  function handleInputChange(
    event: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ) {
    const { name, value } = event.target;

    setActivity({ ...activity, [name]: value });
  }

  if (loadingInitial) {
    return <LoadingComponent content="Loading activity ..." />;
  }

  return (
    <Segment clearing>
      <Form onSubmit={handleSubmit} autoComplete="off">
        <Form.Input
          value={activity.title}
          name="title"
          onChange={handleInputChange}
          placeholder="title"
        ></Form.Input>
        <Form.TextArea
          value={activity.description}
          name="description"
          onChange={handleInputChange}
          placeholder="description"
        />
        <Form.Input
          value={activity.category}
          name="category"
          onChange={handleInputChange}
          placeholder="category"
        ></Form.Input>
        <Form.Input
          type="date"
          value={activity.date}
          name="date"
          onChange={handleInputChange}
          placeholder="date"
        ></Form.Input>
        <Form.Input
          value={activity.city}
          name="city"
          onChange={handleInputChange}
          placeholder="city"
        ></Form.Input>
        <Form.Input
          value={activity.venue}
          name="venue"
          onChange={handleInputChange}
          placeholder="venue"
        ></Form.Input>
        <Button
          loading={loading}
          floated="right"
          positive
          type="submit"
          content="Submit"
        ></Button>
        <Button as={Link} to="/activities" floated="right" type="button" content="Cancel"></Button>
      </Form>
    </Segment>
  );
});
