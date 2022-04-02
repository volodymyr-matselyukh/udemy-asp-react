import { observer } from 'mobx-react-lite';
import React, { ChangeEvent, useState } from 'react';
import { Button, Form, Segment } from 'semantic-ui-react';
import { Activity } from '../../../app/models/activity';
import { useStore } from '../../../app/stores/store';

export default observer(function ActivityForm(){
    const {activityStore} = useStore();
    const {selectedActivity, closeForm, loading, updateActivity, createActivity} = activityStore;
    
    const initialState = selectedActivity ?? {
        id:"",
        title: "",
        category: "",
        description: "",
        date: "",
        city: "",
        venue: ""
    } as Activity;

    const [activity, setActivity] = useState<Activity>(initialState);

    function handleSubmit(){
        if(activity.id)
        {
            updateActivity(activity);
        }
        else {
            createActivity(activity);
        }
    }

    function handleInputChange(event: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>){
        const {name, value} = event.target;

        setActivity({...activity, [name]: value});
    }

    return (
        <Segment clearing>
            <Form onSubmit={handleSubmit} autoComplete="off">
                <Form.Input value={activity.title} name="title" onChange={handleInputChange} placeholder="title"></Form.Input>
                <Form.TextArea value={activity.description} name="description" onChange={handleInputChange} placeholder="description" />
                <Form.Input value={activity.category} name="category" onChange={handleInputChange} placeholder="category"></Form.Input>
                <Form.Input type="date" value={activity.date} name="date" onChange={handleInputChange} placeholder="date"></Form.Input>
                <Form.Input value={activity.city} name="city" onChange={handleInputChange} placeholder="city"></Form.Input>
                <Form.Input value={activity.venue} name="venue" onChange={handleInputChange} placeholder="venue"></Form.Input>
                <Button loading={loading} floated='right' positive type='submit' content='Submit'></Button>
                <Button onClick={closeForm} floated='right' type='button' content='Cancel'></Button>
            </Form>
        </Segment>
    )
})