import { observer } from "mobx-react-lite";
import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { Button, Header, Segment } from "semantic-ui-react";
import LoadingComponent from "../../../app/layout/LoadingComponents";
import { ActivityFormValues } from "../../../app/models/activity";
import { useStore } from "../../../app/stores/store";
import { v4 as uuid } from "uuid";
import { useNavigate } from "react-router-dom";
import { Link } from "react-router-dom";
import { Formik, Form } from "formik";
import * as Yup from "yup";
import MyTextInput from "../../../app/common/form/MyTextInput";
import MyTextArea from "../../../app/common/form/MyTextArea";
import MySelectInput from "../../../app/common/form/MySelectInput";
import { categoryOptions } from "../../../app/common/options/CategoryOptions";
import MyDateInput from "../../../app/common/form/MyDateInput";

export default observer(function ActivityForm() {
	const history = useNavigate();
	const { activityStore } = useStore();
	const {
		loadingInitial,
		updateActivity,
		createActivity,
		loadActivity,
	} = activityStore;
	const { id } = useParams<{ id: string }>();

	const [activity, setActivity] = useState<ActivityFormValues>(new ActivityFormValues());

	const validationSchema = Yup.object({
		title: Yup.string().required("The activity title is required"),
		description: Yup.string().required(
			"The activity description is required"
		),
		date: Yup.string().required("Date is required").nullable(),
		venue: Yup.string().required(),
		city: Yup.string().required(),
		category: Yup.string().required(),
	});

	useEffect(() => {
		if (id) {
			loadActivity(id).then((activity) => {
				if (activity) {
					setActivity(new ActivityFormValues(activity));
				}
			});
		} else {
			setActivity({
				id: "",
				title: "",
				category: "",
				description: "",
				date: null,
				city: "",
				venue: "",
			});
		}
	}, [id, loadActivity]);

	function handleFormSubmit(activity: ActivityFormValues) {
		if (!activity.id) {
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

	if (loadingInitial) {
		return <LoadingComponent content="Loading activity ..." />;
	}

	return (
		<Segment clearing>
			<Header content="Activity Details" sub color="teal" />
			<Formik
				validationSchema={validationSchema}
				enableReinitialize
				initialValues={activity}
				onSubmit={handleFormSubmit}
			>
				{({ handleSubmit, isValid, isSubmitting, dirty }) => (
					<Form
						className="ui form"
						onSubmit={handleSubmit}
						autoComplete="off"
					>
						<MyTextInput name="title" placeholder="Title" />
						<MyTextArea
							rows={3}
							name="description"
							placeholder="Description"
						/>
						<MySelectInput
							options={categoryOptions}
							name="category"
							placeholder="Category"
						/>
						<MyDateInput
							name="date"
							placeholderText="Date"
							showTimeSelect
							timeCaption="time"
							dateFormat="MMM d, yyyy h:mm aa"
						/>
						<Header content="Location Details" sub color="teal" />
						<MyTextInput
							name="city"
							placeholder="City"
						></MyTextInput>
						<MyTextInput
							name="venue"
							placeholder="Venue"
						></MyTextInput>
						<Button
							loading={isSubmitting}
							floated="right"
							positive
							type="submit"
							disabled={isSubmitting || !dirty || !isValid}
							content="Submit"
						></Button>
						<Button
							as={Link}
							to="/activities"
							floated="right"
							type="button"
							content="Cancel"
						></Button>
					</Form>
				)}
			</Formik>
		</Segment>
	);
});
