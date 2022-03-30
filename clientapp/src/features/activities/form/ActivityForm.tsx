import React, { ChangeEvent, useState } from 'react';
import { Button, Form, Segment } from 'semantic-ui-react';
import { Activity } from '../../../app/models/activity';

interface Props {
    activity: Activity | undefined;
    closeForm: () => void;
    createOrEdit: (activity: Activity) => void;
}

export default function ActivityForm({activity: selectedActivity, closeForm, createOrEdit}: Props){
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
        createOrEdit(activity);
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
                <Form.Input value={activity.date} name="date" onChange={handleInputChange} placeholder="date"></Form.Input>
                <Form.Input value={activity.city} name="city" onChange={handleInputChange} placeholder="city"></Form.Input>
                <Form.Input value={activity.venue} name="venue" onChange={handleInputChange} placeholder="venue"></Form.Input>
                <Button floated='right' positive type='submit' content='Submit'></Button>
                <Button onClick={closeForm} floated='right' type='button' content='Cancel'></Button>
            </Form>
        </Segment>
    )
}